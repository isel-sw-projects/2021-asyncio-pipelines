package lookwords.FindBiggestWordStrategies;

import java.util.Map;

public interface FindWords {
    Map<String, Integer> words(String folder, int minLength, int maxLength);
}
