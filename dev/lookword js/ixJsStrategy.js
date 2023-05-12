import { AsyncIterable } from 'ix';
import fs from 'fs/promises';
import path from 'path';
import { promisify } from 'util';



async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await fs.readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const filePath = path.resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectoryGenerator(filePath);
    } else if (dirent.name.endsWith('.txt')) {
      yield filePath;
    }
  }
}

async function* readLines(filePath) {
  const content = await fs.readFile(filePath, 'utf-8');
  yield* content.split('\n');
}

function findBiggestWordInFile(filePath) {
  return AsyncIterable.from(readLines(filePath))
    .skip(14)
    .takeWhile((line) => !line.includes('*** END OF '))
    .filter((line) => line.length > 0)
    .flatMap((line) => line.split(' '))
    .reduce((biggest, curr) => (curr.length > biggest.length ? curr : biggest), '');
}

async function findBiggestWordInDirectory(directoryPath) {
  const files = [];
  for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
    files.push(filePath);
  }

  return await AsyncIterable.from(files)
    .flatMap((filePath) => findBiggestWordInFile(filePath))
    .reduce((biggest, curr) => (curr.length > biggest.length ? curr : biggest), '');
}

(async () => {
  const folderPath = 'C:/Users/e351582/OneDrive - EDP/Desktop/PESSOAL/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
  try {
    console.log('Starting benchmark:');
    console.time('Benchmark');
    const biggestWord = await findBiggestWordInDirectory(folderPath);
    console.timeEnd('Benchmark');
    console.log('Biggest word found:', biggestWord);
  } catch (error) {
    console.error('Error:', error);
  }
})();
