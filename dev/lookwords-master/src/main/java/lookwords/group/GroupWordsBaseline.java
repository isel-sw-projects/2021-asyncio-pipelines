package lookwords.group;

import AsyncFile.AsyncFile;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.*;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutionException;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class GroupWordsBaseline implements GroupWords{
    static int count = 0;
    @Override
    public Map<String, Integer> words(String folder, int minLength, int maxLength) {

        ConcurrentHashMap<String, Integer> words = new ConcurrentHashMap<>();
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            List<Path> pathsList = paths.filter(Files::isRegularFile).collect(toList());

            LinkedList<CompletableFuture<LinkedList<String>>> allFuturesList = new LinkedList<>();

            for (int i = 0; i < pathsList.size(); i++) {


                allFuturesList.add(getFileWordsList(pathsList.get(i), minLength, maxLength, words));
            }

            for(int y = 0; y < allFuturesList.size(); y++) {
                allFuturesList.get(y).join();
            }

        } catch (ExecutionException e) {
            throw new RuntimeException(e);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        return words;
    }



    public CompletableFuture getFileWordsList(Path file, int wordMinLength, int wordMaxLength, ConcurrentHashMap<String, Integer> words) throws ExecutionException, InterruptedException {

        Boolean[] ignoreLine = {false};

        return CompletableFuture.runAsync(() -> {
            CompletableFuture readAllLines = AsyncFile.readAllLines(file, (line, c) -> {

                if (line.contains("*** END OF ")) {
                    ignoreLine[0] = true;
                }
                if (ignoreLine[0]) {
                    return;
                }

                String[] wordsInLine = line.split(" ");

                for(int i = 0; i < wordsInLine.length; i++) {
                    if(wordsInLine[i].length() > wordMinLength && wordsInLine[i].length() < wordMaxLength) {
                        words.merge(wordsInLine[i], 1, Integer::sum);
                    }
                }
            });

            readAllLines.join();
        });
    }
}
