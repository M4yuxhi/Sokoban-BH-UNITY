using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEstrella : MonoBehaviour
{
    public LevelNode? root;
    public List<KeyValuePair <LevelNode,int>> Open;
    public List<KeyValuePair<LevelNode, int>> Close;


    public AEstrella(LevelNode root) 
    {
        this.root = root;
        Open = new List<KeyValuePair<LevelNode, int>>();
        Close = new List<KeyValuePair<LevelNode, int>>();
        Open.Add(new KeyValuePair<LevelNode,int>(this.root,0));
    }


    public int Heuristica()
    {
        int h = 0;
        //TO DO: que haga algo 
        return h;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
