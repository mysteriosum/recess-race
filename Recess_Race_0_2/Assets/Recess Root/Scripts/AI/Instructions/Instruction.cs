using UnityEngine;
using System.Collections;

public abstract class Instruction {

    public Instruction nextInstruction;
    public Agent agent;
    public bool isDone { get; protected set;}

    public Instruction(Agent agent) {
        this.agent = agent;
        isDone = false;
    }

    public Instruction(Agent agent, Instruction nextInstruction) {
        this.agent = agent;
        this.nextInstruction = nextInstruction;
        isDone = false;
    }

    abstract public void start();
    abstract public void update();
}
