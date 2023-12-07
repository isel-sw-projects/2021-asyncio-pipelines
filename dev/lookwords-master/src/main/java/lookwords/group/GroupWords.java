package lookwords.group;

import java.nio.file.Path;
import java.util.Map;

public interface GroupWords {
    Map<String, Integer> words(Path folder, int minLength, int maxLength);

}
