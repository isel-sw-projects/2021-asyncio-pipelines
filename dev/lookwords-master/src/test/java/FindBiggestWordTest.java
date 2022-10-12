import lookwords.Benchmark.FindBiggestWordBench;
import lookwords.Benchmark.FindBiggestWordParallelBench;
import org.junit.Test;

import static org.junit.Assert.assertEquals;

public class FindBiggestWordTest {

    @Test
    public void testFindBiggestWordInFolderqutyUsingFlux() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordRxIoInFlux();
        assertEquals(53, word.length());
        assertEquals("amaiorpalavraalgumavezfeitahooomasesperaistoeumafrase", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingObservale() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordRxIoInObservable();
        assertEquals(53, word.length());
        assertEquals("amaiorpalavraalgumavezfeitahooomasesperaistoeumafrase", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingMultithread() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordReaderInMultithread();
        assertEquals(53, word.length());
        assertEquals("amaiorpalavraalgumavezfeitahooomasesperaistoeumafrase", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingStream() {
        String word = new FindBiggestWordBench("guty").FindBiggestWordRxIoInStream();
        assertEquals(53, word.length());
        assertEquals("amaiorpalavraalgumavezfeitahooomasesperaistoeumafrase", word);
    }

    @Test
    public void testFindBiggestWordInFolderqutyUsingParallelObservale() {
        String word = new FindBiggestWordParallelBench("guty").FindBiggestWordParallelRxIoInObservable();
        assertEquals(53, word.length());
        assertEquals("amaiorpalavraalgumavezfeitahooomasesperaistoeumafrase", word);
    }


    @Test
    public void testFindBiggestWordInFolderqutyUsingParallelFlux() {
        String word = new FindBiggestWordParallelBench("guty").FindBiggestWordParallelRxIoInFlux();
        assertEquals(53, word.length());
        assertEquals("amaiorpalavraalgumavezfeitahooomasesperaistoeumafrase", word);
    }

    public void testFindBiggestParallelWordInFolderqutyUsingParallelStream() {
        String word = new FindBiggestWordParallelBench("guty").FindBiggestWordParallelRxIoInStream();
        assertEquals(53, word.length());
        assertEquals("amaiorpalavraalgumavezfeitahooomasesperaistoeumafrase", word);
    }
}
