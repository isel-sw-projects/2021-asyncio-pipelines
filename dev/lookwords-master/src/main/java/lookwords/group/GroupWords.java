package lookwords.group;

import java.util.Map;

public interface GroupWords {
    Map<String, Integer> words(String folder, int minLength, int maxLength);
}
