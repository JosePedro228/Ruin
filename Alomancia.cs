using UnityEngine;
using System;
public class Alomancia : MonoBehaviour
{
    public Transform player;
    private bool isAttracting = false;
    public float attractionForce = 10f;
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    private RaycastHit2D hit;
    private GameObject metalCathed;
    private Rigidbody2D rbMetal;
    private Metal objetoMetal;
    private Rigidbody2D rbPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rbPlayer = GetComponent<Rigidbody2D>(); 
    }


    // Update � chamado uma vez por frame
    void Update()
    {
      
        // Verifica se o bot�o esquerdo do mouse foi pressionado
        if (Input.GetMouseButtonDown(0))
        {
            // Obt�m a posi��o do mouse em coordenadas de tela
            mousePosition = Input.mousePosition;

            // Converte a posi��o do mouse para coordenadas do mundo
            worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0; // Define z como 0 para 2D

            // Realiza um Raycast 2D na posi��o do clique
            hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            // Verifica se o Raycast atingiu algum objeto
            if (hit.collider != null && (hit.collider.CompareTag("MetalColect") || hit.collider.CompareTag("MetalNotColect")))
            {
            isAttracting = true; // Alterna o estado de atra��o
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isAttracting = false; // Desativa a atra��o ao soltar o bot�o
            Debug.Log("Bot�o esquerdo do mouse foi solto.");
            if(metalCathed != null)
            {
                mousePosition = Input.mousePosition;
                objetoMetal.isGrapped=false;
                // Converte a posi��o do mouse para coordenadas do mundo
                worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.z = 0; // Define z como 0 para 2D

                // Realiza um Raycast 2D na posi��o do clique
                hit = Physics2D.Raycast(worldPosition, Vector2.zero);

                Vector2 dropPosition = player.position + (worldPosition - player.position).normalized *0.5f;
                Vector3 vector3= new Vector3(dropPosition.x, dropPosition.y, 0);
                metalCathed.SetActive(true); // Reativa o objeto coletado na cena
                metalCathed.transform.position = vector3; // Define a posi��o do objeto coletado
              //  Instantiate(metalCathed, vector3, Quaternion.identity);
               // Debug.Log("Metal coletado: " + metalCathed.ToString());
                metalCathed = null; // Reseta o objeto coletado ap�s processar
            }
        }
        if (isAttracting)
        {
            objetoMetal = hit.collider.gameObject.GetComponent<Metal>();
            
            objetoMetal.isGrapped=true;
            rbMetal = objetoMetal.GetComponent<Rigidbody2D>();
            rbMetal.AddForce((player.position - objetoMetal.transform.position).normalized * rbPlayer.mass);
            if (rbMetal.linearVelocity == Vector2.zero)
            {
                rbPlayer.AddForce((objetoMetal.transform.position - player.position).normalized * rbPlayer.mass);
            }else{
                rbPlayer.AddForce((objetoMetal.transform.position - player.position).normalized * Math.Min(rbMetal.mass, rbPlayer.mass));
                }
            
           // Debug.Log("Clicou no objeto: " + (player.position - objetoMetal.transform.position));
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MetalColect") && objetoMetal.isGrapped)
        {
            // C�digo para coletar o metal
            metalCathed = collision.gameObject; // Armazena o objeto coletado
            metalCathed.SetActive(false); // Desativa o objeto coletado na cena
            //Destroy(collision.gameObject); // Exemplo: destruir o objeto coletado
            isAttracting = false; // Para de atrair ap�s coletar
        }
    }
}
