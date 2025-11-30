public interface IMindProgressUpdater
{
    void StartFarming();
    void StopFarming();
    void BlockFarming(bool isBlock);
    void Redraw();
}
