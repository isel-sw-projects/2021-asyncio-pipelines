package lookwords.FindBiggestWordStrategies;

import lookwords.FileUtils;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Map;
import java.util.Optional;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.lines;
import static lookwords.FileUtils.pathFrom;

//public class FindWordBlockingReaderInMultiThread implements FindBiggestWord {
//    /**
//     * We use a lazy approach trying to avoid intermediate data structures.
//     * !!! To do: try it eagerly and check differences
//     */
//    Object mon = new Object();
//
//    public final Containner<String> findBiggestWord(String folder) {
//        final int THREAD_POOL_SIZE = Runtime.getRuntime().availableProcessors();
//        final ExecutorService EXEC = Executors.newFixedThreadPool(THREAD_POOL_SIZE);
//        Containner cont = new Containner("");
//        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
//
//            paths
//                .filter(Files::isRegularFile)
//                .map(file -> EXEC.submit(() -> lines(file, cont)))
//                    .forEach(word -> {
//                        synchronized (mon) {
//                            if (cont.value.length() < word.length()) {
//                                cont.value = word;
//                            }
//                        }
//                    })
//                .collect(toList())
//                .stream()
//                .forEach(FileUtils::join);
//            return cont;
//        } catch (IOException e) {
//            throw new UncheckedIOException(e);
//        } finally {
//            EXEC.shutdown();
//        }
//    }
//}
