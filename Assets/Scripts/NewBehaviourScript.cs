using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;
public sealed class NewBehaviourScript : MonoBehaviour
{
    private const string _ConfigText = @"
input_stream: ""in""
output_stream: ""out""
node {
    calculator: ""PassThroughCalculator""
    input_stream: ""in""
    output_stream: ""out1""
}
node {
    calculator: ""PassThroughCalculator""
    input_stream: ""out1""
    output_stream: ""out""
}
";
    // Start is called before the first frame update
    void Start()
    {
        var graph = new CalculatorGraph(_ConfigText);
        var poller = graph.AddOutputStreamPoller<string>("out").Value();
        graph.StartRun();

        for (var i = 0; i < 10; i++)
        {
            graph.AddPacketToInputStream("in", new StringPacket("Hello World!", new Timestamp(i))).AssertOk();
        }

        graph.CloseInputStream("in").AssertOk();
        var packet = new StringPacket();

        while (poller.Next(packet))
        {
            Debug.Log(packet.Get());
        }
        graph.WaitUntilDone().AssertOk();
    }
}
