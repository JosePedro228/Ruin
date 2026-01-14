using UnityEngine;
using System;
public class Alomancia : MonoBehaviour
{
    public Transform player;
    public float deadZone = 0.05f;
    private bool isRepeling = false;
    private bool isAttracting = false;
    public float attractionForce = 10f;
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    private RaycastHit2D hitA;
    private RaycastHit2D hitR;
    private GameObject metalCathed;
    private Rigidbody2D rbMetalRepel;

    private Rigidbody2D rbMetalAttrac;
    private Metal objetoMetalAttrac;
    private Metal objetoMetalRepel;
    private Rigidbody2D rbPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
    }


    // Update � chamado uma vez por frame
    void Update()
    {
        // Obt�m a posi��o do mouse em coordenadas de tela
        mousePosition = Input.mousePosition;

        // Converte a posi��o do mouse para coordenadas do mundo
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // Define z como 0 para 2D
        // Verifica se o botão esquerdo do mouse foi pressionado
        if (Input.GetMouseButtonDown(1))
        {
            // Realiza um Raycast 2D na posi��o do clique
            hitR = Physics2D.Raycast(worldPosition, Vector2.zero);

            // Verifica se o Raycast atingiu algum objeto
            if (hitR.collider != null && (hitR.collider.CompareTag("MetalColect") || hitR.collider.CompareTag("MetalNotColect")))
            {
                isRepeling = true; // Alterna o estado de repulso
            }
        }
        if (Input.GetMouseButtonDown(0))
        {


            // Realiza um Raycast 2D na posi��o do clique
            hitA = Physics2D.Raycast(worldPosition, Vector2.zero);

            // Verifica se o Raycast atingiu algum objeto
            if (hitA.collider != null && (hitA.collider.CompareTag("MetalColect") || hitA.collider.CompareTag("MetalNotColect")))
            {
                isAttracting = true; // Alterna o estado de atrao
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRepeling = false; // Desativa a repulso ao soltar o bot�o
            Debug.Log("Bot�o direito do mouse foi solto.");
            if (objetoMetalRepel != null)
            {

                objetoMetalRepel = null;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isAttracting = false; // Desativa a atra��o ao soltar o bot�o
            Debug.Log("Bot�o esquerdo do mouse foi solto.");
       
            if (metalCathed != null)
            {
                mousePosition = Input.mousePosition;
                objetoMetalAttrac.isGrapped = false;
                // Converte a posi��o do mouse para coordenadas do mundo
               /* worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.z = 0; // Define z como 0 para 2D*/

                // Realiza um Raycast 2D na posi��o do clique
                hitA = Physics2D.Raycast(worldPosition, Vector2.zero);

                Vector2 dropPosition = player.position + (worldPosition - player.position).normalized * 0.5f;
                Vector3 vector3 = new Vector3(dropPosition.x, dropPosition.y, 0);
                metalCathed.SetActive(true); // Reativa o objeto coletado na cena
                metalCathed.transform.position = vector3; // Define a posi��o do objeto coletado
                                                          //  Instantiate(metalCathed, vector3, Quaternion.identity);
                                                          // Debug.Log("Metal coletado: " + metalCathed.ToString());
                metalCathed = null; // Reseta o objeto coletado ap�s processar
            }
                 if (objetoMetalAttrac != null)
            {

                objetoMetalAttrac = null;
            }
        }
        if (isAttracting)
        {
            objetoMetalAttrac = hitA.collider.gameObject.GetComponent<Metal>();

            objetoMetalAttrac.isGrapped = true;
            rbMetalAttrac = objetoMetalAttrac.GetComponent<Rigidbody2D>();
            rbMetalAttrac.AddForce((player.position - objetoMetalAttrac.transform.position).normalized * rbPlayer.mass);
            if (rbMetalAttrac.linearVelocity.magnitude <= deadZone)
            {
                rbPlayer.AddForce((objetoMetalAttrac.transform.position - player.position).normalized * rbPlayer.mass);
            }
            else
            {
                rbPlayer.AddForce((objetoMetalAttrac.transform.position - player.position).normalized * Math.Min(rbMetalAttrac.mass, rbPlayer.mass));
            }

            // Debug.Log("Clicou no objeto: " + (player.position - objetoMetal.transform.position));
        }
        if (isRepeling)
        {
            objetoMetalRepel = hitR.collider.gameObject.GetComponent<Metal>();
            rbMetalRepel = objetoMetalRepel.GetComponent<Rigidbody2D>();
            rbMetalRepel.AddForce((objetoMetalRepel.transform.position - player.position).normalized * rbPlayer.mass);
            if (rbMetalRepel.linearVelocity.magnitude <= deadZone)
            {
                rbPlayer.AddForce((player.position - objetoMetalRepel.transform.position).normalized * rbPlayer.mass);
            }
            else
            {
                rbPlayer.AddForce((player.position - objetoMetalRepel.transform.position).normalized * Math.Min(rbMetalRepel.mass, rbPlayer.mass));
            }
            // Debug.Log("Clicou no objeto: " + (player.position - objetoMetal.transform.position));
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MetalColect") && objetoMetalAttrac.isGrapped)
        {
            // C�digo para coletar o metal
            metalCathed = collision.gameObject; // Armazena o objeto coletado
            metalCathed.SetActive(false); // Desativa o objeto coletado na cena
            //Destroy(collision.gameObject); // Exemplo: destruir o objeto coletado
            isAttracting = false; // Para de atrair ap�s coletar
        }
    }
}
