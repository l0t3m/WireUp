using System.Collections.Generic;
using UnityEngine;

public enum BlockSection
{
    Empty,
    StraightSection,
    LeftCornerSection,
    TSection,
    RightCornerSection,
    Obstacle,
    StartSection,
    FinishSection
}

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField] Object GridBlock;
    [SerializeField] Object Obstacle;
    [SerializeField] Object StraightSection;
    [SerializeField] Object LeftCornerSection;
    [SerializeField] Object TSection;
    [SerializeField] Object RightCornerSection;
    [SerializeField] Object StartSection;
    [SerializeField] Object FinishSection;


    public Object GetGridObject()
    {
        return GridBlock;
    }

    public Object GetBlockByType(BlockSection blockType)
    {
        switch (blockType)
        {
            case BlockSection.StraightSection:
                return StraightSection;
            case BlockSection.TSection:
                return TSection;
            case BlockSection.RightCornerSection:
                return RightCornerSection;
            case BlockSection.LeftCornerSection:
                return LeftCornerSection;
            case BlockSection.Obstacle:
                return Obstacle;
            case BlockSection.StartSection:
                return StartSection;
            case BlockSection.FinishSection:
                return FinishSection;
            default: return null;
        }
    }
}
