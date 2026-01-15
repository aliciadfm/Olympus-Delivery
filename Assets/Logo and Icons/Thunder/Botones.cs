using UnityEngine;

public class BotonAccion : MonoBehaviour
{
    public LogicaThunder controlador;
    public bool esBotonCorazon;

    [Header("Ajustes de Color")]
    public Color colorHover = new Color(0.8f, 0.8f, 0.8f);
    public Color colorClick = new Color(0.5f, 0.5f, 0.5f);

    private MeshRenderer miRender;
    private Color colorOriginal;

    void Start() {
        miRender = GetComponent<MeshRenderer>();
        colorOriginal = miRender.material.color;
    }

    private void OnMouseEnter() {
        miRender.material.color = colorHover;
    }

    private void OnMouseExit() {
        miRender.material.color = colorOriginal;
    }

    private void OnMouseDown() {
        miRender.material.color = colorClick;
        controlador.ValidarDecision(esBotonCorazon);
    }

    private void OnMouseUp() {
        miRender.material.color = colorHover;
    }
}