using PrimeTween;
using UnityEngine;

public class ObjectPartMaterialPropertyBlock : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private string alphaValueReference;

    #region PRIVATE FIELD
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    #endregion

    private void Awake()
    {
        BasicObjectPart.selectObjectPartEvent += OnObjectPartSelected;
        BasicObjectPart.deselectObjectPartEvent += OnObjectPartDeselected;

        Init();
    }

    void OnDestroy()
    {
        BasicObjectPart.selectObjectPartEvent -= OnObjectPartSelected;
        BasicObjectPart.deselectObjectPartEvent -= OnObjectPartDeselected;
    }

    private void Init()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
    }

    public void SetAlpha(float alpha)
    {
        Init();

        _propertyBlock.SetFloat(alphaValueReference, alpha);

        _renderer.SetPropertyBlock(_propertyBlock);
    }

    public void OnObjectPartSelected(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Tween.Custom(1, 0.5f, duration: 0.3f, onValueChange: newVal =>
            {
                _propertyBlock.SetFloat(alphaValueReference, newVal);

                _renderer.SetPropertyBlock(_propertyBlock);
            });
        }
    }

    public void OnObjectPartDeselected(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Tween.Custom(0.5f, 1, duration: 0.3f, onValueChange: newVal =>
            {
                _propertyBlock.SetFloat(alphaValueReference, newVal);

                _renderer.SetPropertyBlock(_propertyBlock);
            });
        }
    }
}
