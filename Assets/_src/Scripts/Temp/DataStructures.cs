using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStructures : MonoBehaviour
{
    private Stack<int> integerStack;
    private List<int> integerList;
    private Queue<int> integerQueue;
    private Dictionary<string, int> valueDictionary;

    private void Start()
    {
        ListPrintExample();
        StackPrintExample();
        QueuePrintExample();
        DictionaryPrintExample();
    }

    void ListPrintExample()
    {
        integerList = new List<int>();
        integerList.Add(3);

        

        string listOutput = "List: ";
        InsertTextIntoBrackets("OPEN", ref listOutput);
        foreach (var item in integerList)
        {
            listOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref listOutput);
        print(listOutput);

        integerList.Add(5);
        listOutput = "List: ";

        InsertTextIntoBrackets("OPEN", ref listOutput);
        foreach (var item in integerList)
        {
            listOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref listOutput);
        print(listOutput);
        integerList.Remove(5);
        listOutput = "List: ";

        InsertTextIntoBrackets("OPEN", ref listOutput);
        foreach (var item in integerList)
        {
            listOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref listOutput);
        print(listOutput);


    }
    void StackPrintExample()
    {
        integerStack = new Stack<int>();
        integerStack.Push(3);

        string stackOutput = "Stack: ";
        InsertTextIntoBrackets("OPEN", ref stackOutput);
        foreach (var item in integerStack)
        {
            stackOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref stackOutput);
        print(stackOutput);
        integerStack.Push(5);

        stackOutput = "Stack: ";
        InsertTextIntoBrackets("OPEN", ref stackOutput);
        foreach (var item in integerStack)
        {
            stackOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref stackOutput);
        print(stackOutput);
        stackOutput = "Stack: ";
        integerStack.Pop();
        InsertTextIntoBrackets("OPEN", ref stackOutput);
        foreach (var item in integerStack)
        {
            stackOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref stackOutput);
        print(stackOutput);
    }
    void QueuePrintExample()
    {
        integerQueue = new Queue<int>();
        integerQueue.Enqueue(3);

        string queueOutput = "Queue: ";
        InsertTextIntoBrackets("OPEN", ref queueOutput);
        foreach (var item in integerQueue)
        {
            queueOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref queueOutput);
        print(queueOutput);

        integerQueue.Enqueue(5);
        queueOutput = "Queue: ";

        InsertTextIntoBrackets("OPEN", ref queueOutput);
        foreach (var item in integerQueue)
        {
            queueOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref queueOutput);
        print(queueOutput);
        integerQueue.Dequeue();
        queueOutput = "Queue: ";

        InsertTextIntoBrackets("OPEN", ref queueOutput);
        foreach (var item in integerQueue)
        {
            queueOutput += item.ToString() + " ";
        }
        InsertTextIntoBrackets("CLOSE", ref queueOutput);
        print(queueOutput);


    }
    void DictionaryPrintExample()
    {
        valueDictionary = new Dictionary<string, int>();
        valueDictionary.Add("three", 3);

        string dictionaryOutput = "Dictionary: ";

        InsertTextIntoBrackets("OPEN", ref dictionaryOutput);
        foreach (var item in valueDictionary)
        {
            dictionaryOutput += $"{item.Value}({item.Key}) ";
        }
        InsertTextIntoBrackets("CLOSE", ref dictionaryOutput);
        print(dictionaryOutput);

        valueDictionary.Add("five", 5);
        dictionaryOutput = "Dictionary: ";

        InsertTextIntoBrackets("OPEN", ref dictionaryOutput);
        foreach (var item in valueDictionary)
        {
            dictionaryOutput += $"{item.Value}({item.Key}) ";
        }
        InsertTextIntoBrackets("CLOSE", ref dictionaryOutput);
        print(dictionaryOutput);

        valueDictionary.Remove("five");

        dictionaryOutput = "Dictionary: ";

        InsertTextIntoBrackets("OPEN", ref dictionaryOutput);
        foreach (var item in valueDictionary)
        {
            dictionaryOutput += $"{item.Value}({item.Key}) ";
        }
        InsertTextIntoBrackets("CLOSE", ref dictionaryOutput);
        print(dictionaryOutput);



    }
    void InsertTextIntoBrackets(string bracketsType, ref string stringToRecieve)
    {
        switch (bracketsType.ToUpper())
        {
            case "CLOSE":
                stringToRecieve += "}";
                break;
            case "OPEN":
                stringToRecieve += "{ ";
                break;
            default:
                break;
        }
        
    }
}
