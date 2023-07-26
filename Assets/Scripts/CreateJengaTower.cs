using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateJengaTower : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Original Jenga Piece to be duplicated and with IsActive = false")]
    private GameObject originalPiece;
    [SerializeField]
    [Tooltip("Total rows number of the tower")]
    private int rowNumber = 5;
    [SerializeField]
    [Tooltip("Short face width taken from unity GameObject")]
    private float widthPiece = 0.0f;
    [SerializeField]
    [Tooltip("Distance between pieces")]
    private float separationBetweenPieces = 0.0f;
    [SerializeField]
    [Tooltip("Amount of mass to reduce for each row")]
    private float reduceAmount = 0.0001f;
    [SerializeField]
    [Tooltip("Amount of mass to reduce for each move")]
    private float reduceAmountOnDrop = 2.0f;
    
    private RectTransform rtransform;
    private List<GameObject> tower = new List<GameObject>();

    // Almost all tricks was canceled fixing Project Settings->Phisics->Enable Adaptive forces

    // Start is called before the first frame update
    void Start()
    {
        GameObject clone = null;

        //Rows
        int row = 0;
        bool rotate = false;
        for (int j = 0; j < rowNumber; j++)
        {
            var y = originalPiece.transform.position.y + (j * widthPiece);
            rotate = (j % 2 == 0);
            var reduce = 0.0f;
            // each row
            for (int i = 0; i < 3; i++)
            {
                clone = GameObject.Instantiate(originalPiece);                
                
                if(rotate)
                {
                    var z = originalPiece.transform.position.z + (i * widthPiece);
                    clone.transform.position = new Vector3(originalPiece.transform.position.x+widthPiece, 
                                            j == 0 ? y : (y + (j * separationBetweenPieces)), 
                                            i == 0 ? z- widthPiece : (z + (i * separationBetweenPieces) - widthPiece)); // Fixed to re position rotared piece
                    clone.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
                }
                else
                {
                    var x = originalPiece.transform.position.x + (i * widthPiece);
                    clone.transform.position = new Vector3(i == 0 ? x : (x + (i * separationBetweenPieces)), 
                                            j == 0 ? y : (y + (j * separationBetweenPieces)), 
                                            originalPiece.transform.position.z);
                }

                //clone.GetComponent<Rigidbody>().mass -= reduce;
                //clone.GetComponent<Rigidbody>().AddForce(Physics.gravity * (clone.GetComponent<Rigidbody>().mass * clone.GetComponent<Rigidbody>().mass));

                clone.SetActive(true); 
                tower.Add(clone);
            }
            reduce = j * reduceAmount;
            row++;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScene()
    {
        tower.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // Reduce vertical forces that smash the tower while the piece is grabbed
    public void SetKinematic(GameObject current)
    {
        foreach(var j in tower)
        {
            if(j != current)
            {
                j.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    // Return physic parameters by default
    public void UnsetKinematic(GameObject current)
    {
        // trick to reduce weight of the pieces on top of the tower
        //current.GetComponent<Rigidbody>().mass = (current.GetComponent<Rigidbody>().mass / reduceAmountOnDrop);
        //current.GetComponent<Rigidbody>().AddForce(Physics.gravity * (current.GetComponent<Rigidbody>().mass * current.GetComponent<Rigidbody>().mass));

        foreach(var j in tower)
        {
            j.GetComponent<Rigidbody>().isKinematic = false;            
        }
        
    }

}
