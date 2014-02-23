using UnityEngine;
using System.Collections;

public abstract class Instruction {

    public Instruction nextInstruction;
    public Agent agent;
    public bool isDone { get; protected set;}

    public Instruction(Agent agent) {
        this.agent = agent;
    }

    public Instruction(Agent agent, Instruction nextInstruction) {
        this.agent = agent;
        this.nextInstruction = nextInstruction;
    }

    abstract public void start();
    abstract public void update();
}
