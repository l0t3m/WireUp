using UnityEngine;

[CreateAssetMenu(fileName = "SnapCorrelation", menuName = "Scriptable Objects/SnapCorrelation")]
public class SnapCorrelation : ScriptableObject
{
    public Snap grid;
    public DragAndDrop block;

    public SnapCorrelation(Snap grid, DragAndDrop block)
    {
        this.grid = grid;
        this.block = block;
    }

    public void ExecuteSnap()
    {
        grid.DoSnap(block);
        block.DoSnap();
    }

    public void ExecuteUnsnap()
    {
        grid.DoUnsnap(block);
        block.DoUnsnap();
    }

    public override bool Equals(object other)
    {
        if (other is Snap grid)
            return this.grid.Equals(grid);
        else if (other is DragAndDrop block)
            return this.block.Equals(block);
        else return base.Equals(other);
    }
}
