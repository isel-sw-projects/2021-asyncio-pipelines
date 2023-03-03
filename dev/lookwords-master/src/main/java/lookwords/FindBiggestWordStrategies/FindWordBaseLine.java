package lookwords.FindBiggestWordStrategies;



import AsyncFile.AsyncFile;
import lookwords.FindBiggestWithParallel.FindBiggestWordConcurrent;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.*;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class FindWordBaseLine implements FindBiggestWordConcurrent {

    public String findBiggestWord(String folder) {
        String biggestWord = "";
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            List<Path> pathsList = paths.filter(Files::isRegularFile).collect(toList());

            LinkedList<CompletableFuture<String>> allFuturesList = new LinkedList<>();

            for (int i = 0; i < pathsList.size(); i++) {
                allFuturesList.add(findBiggestWordInFile(pathsList.get(i)));
            }

           for(int y = 0; y < allFuturesList.size(); y++) {
               String currWord = allFuturesList.get(y).join();
               if(biggestWord.length() < currWord.length()) {
                   biggestWord = currWord;
               }
           }

        } catch (ExecutionException e) {
            throw new RuntimeException(e);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        return biggestWord;
    }


    public CompletableFuture<String> findBiggestWordInFile(Path file) throws ExecutionException, InterruptedException {

        Boolean[] ignoreLine = {false};
        String[] word = {""};

       return CompletableFuture.supplyAsync(() -> {

            CompletableFuture readAllLines = AsyncFile.readAllLines(file, (line, c) -> {

                if (line.contains("*** END OF ")) {
                    ignoreLine[0] = true;
                }
                if (ignoreLine[0]) {
                    return;
                }

                String[] wordsInLine = line.replaceAll("[^a-zA-Z ]", "").split(" ");

                for (int y = 0; y < wordsInLine.length; y++) {
                    if (wordsInLine[y].length() > word[0].length()) {
                        word[0] = wordsInLine[y];
                    }
                }
            });

            readAllLines.join();

            return word[0];
        });
    }
}