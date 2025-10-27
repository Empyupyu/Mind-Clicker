public interface IMindProgressService
{
    void StartFarming();
    void StopFarming();
    void BlockFarming(bool isBlock);
    void Redraw();
}
