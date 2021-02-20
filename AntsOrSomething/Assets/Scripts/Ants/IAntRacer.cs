using System.Collections;

public interface IAntRacer
{
    IEnumerator BeginRace();
    
    IEnumerator Countdown();
    IEnumerator NewCheckpoint(uint index);
    IEnumerator NewLap(int index, uint count);

    IEnumerator Finished();
}