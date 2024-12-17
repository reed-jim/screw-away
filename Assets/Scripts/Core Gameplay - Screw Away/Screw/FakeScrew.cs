using UnityEngine;

public class FakeScrew : BasicScrew
{
    public void CloneFromScrew(BaseScrew screw)
    {
        {
            transform.position = screw.transform.position;
            transform.localScale = screw.transform.localScale;
            transform.rotation = screw.transform.rotation;

            screwServiceLocator.screwMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(screw.Faction));
        }
    }
}
