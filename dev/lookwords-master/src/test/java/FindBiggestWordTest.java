import lookwords.Benchmark.FindBiggestWordBench;
import org.junit.Test;

import static org.junit.Assert.assertEquals;

public class FindBiggestWordTest {

    @Test
    public void testFindBiggestWordInFolderqutyUsingFlux() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordRxIoInFlux();
        assertEquals(16, word.length());
        assertEquals("DesbordesValmore", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingObservale() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordRxIoInObservable();
        assertEquals(16, word.length());
        assertEquals("DesbordesValmore", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingMultithread() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordReaderInMultithread();
        assertEquals(16, word.length());
        assertEquals("DesbordesValmore", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingStream() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordRxIoInStream();
        assertEquals(16, word.length());
        assertEquals("DesbordesValmore", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingKtFlow() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordReaderInFlow();
        assertEquals(16, word.length());
        assertEquals("DesbordesValmore", word);
    }
}
