public class SnapCorrelation
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
        grid.DoUnsnap();
        block.DoUnsnap();
    }

    public bool IsPartOfCorrelation(object obj)
    {
        return obj.Equals(block) || obj.Equals(grid);
    }
}
