package lookwords.FindBiggestWordStrategies;



import AsyncFile.AsyncFile;
import lookwords.FindBiggestWithParallel.FindBiggestWordConcurrent;

import java.io.IOException;
import java.io.UncheckedIOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.concurrent.*;
import java.util.stream.Stream;

import static java.util.stream.Collectors.toList;
import static lookwords.FileUtils.pathFrom;

public class FindWordBaseLine implements FindBiggestWordConcurrent {
    String [] word = {""};

    public String findBiggestWord(String folder) {
        String biggestWord = "";
        try (Stream<Path> paths = Files.walk(pathFrom(folder))) {
            List<Path> pathsList = paths.filter(Files::isRegularFile).collect(toList());


            for (int i = 0; i < pathsList.size(); i++) {
                String curr = findWordInFile(pathsList.get(i));
                if(curr.length() > biggestWord.length()) {
                    biggestWord = curr;
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


    public String findWordInFile(Path file) throws ExecutionException, InterruptedException {

        Boolean [] ignoreLine = { false};
        AsyncFile.readAllLines(file, (line, c) -> {

            if (line.contains("*** END OF ")) {
                ignoreLine[0] = true;
            }
            if(ignoreLine[0]){
                return;
            }

            String[] wordsInLine = line.replaceAll("[^a-zA-Z ]", "").split(" ");

            for (int y = 0; y < wordsInLine.length; y++) {
                if (wordsInLine[y].length() > word[0].length()) {
                    word[0] = wordsInLine[y];
                }
            }
        }).join();

        return word[0];
    }
}