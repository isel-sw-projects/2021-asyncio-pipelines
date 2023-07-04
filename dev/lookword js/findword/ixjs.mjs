import fs from 'fs';
import path from 'path';
import { promisify } from 'util';
import { from } from 'ix/iterable';
import { filter, flatMap, map, maxBy, skip, takeWhile } from 'ix/iterable/operators';

const readFile = promisify(fs.readFile);
const readdir = promisify(fs.readdir);

const lineSeparator = '\n';
const wordSeparator = ' ';
const textFileExtension = '.txt';

async function* getFilesFromDirectory(dir) {
  const dirents = await readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const res = path.resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectory(res);
    } else if (dirent.isFile() && dirent.name.endsWith(textFileExtension)) {
      yield res;
    }
  }
}

function fileLines(filePath) {
  const data = fs.readFileSync(filePath, 'utf8');
  return from(data.split(lineSeparator));
}

function findBiggestWordInFile(filePath) {
  return fileLines(filePath).pipe(
    skip(14),
    takeWhile((line) => !line.includes('*** END OF ')),
    filter((line) => line.length > 0),
    flatMap((line) => line.split(wordSeparator)),
    maxBy((word) => word.length),
  );
}

async function findBiggestWordInDirectory(directoryPath) {
  let longestWord = '';
  for await (const filePath of getFilesFromDirectory(directoryPath)) {
    const word = findBiggestWordInFile(filePath);
    if (word.length > longestWord.length) {
      longestWord = word;
    }
  }
  return longestWord;
}

async function benchmark() {
  const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
  try {
    console.time('Benchmark');
    const longestWord = await findBiggestWordInDirectory(folderPath);
    console.timeEnd('Benchmark');
    console.log(`The longest word is: ${longestWord}`);
  } catch (error) {
    console.error('Error:', error);
  }
}

benchmark();
export default benchmark;
