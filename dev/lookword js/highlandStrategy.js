import fs from 'fs';
import path from 'path';
import { promises as fsPromises } from 'fs';
import H from 'highland';

const { readdir } = fsPromises;

async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const filePath = path.resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectoryGenerator(filePath);
    } else if (dirent.name.endsWith('.txt')) {
      yield filePath;
    }
  }
}

function findBiggestWordInFile(filePath) {
  const stream = H(fs.createReadStream(filePath, 'utf-8')).split();

  return stream
    .drop(14)
    .filter((line) => !line.includes('*** END OF '))
    .map((line) => line.split(' '))
    .flatten()
    .reduce1((biggest, curr) => (curr.length > biggest.length ? curr : biggest));
}

function findBiggestWordInDirectory(directoryPath) {
  const fileStream = H(async (push, next) => {
    for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
      push(null, filePath);
    }
    push(null, H.nil);
  });
  return fileStream.flatMap(findBiggestWordInFile)
    .reduce1((biggest, curr) => (curr.length > biggest.length ? curr : biggest));
}

function benchmark() {
  const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
  try {
    console.log('Starting benchmark baseline with all file to memory:');
    console.time('Benchmark');
    
    findBiggestWordInDirectory(folderPath)
      .toArray((biggestWords) => {
        const biggestWord = biggestWords.reduce((prev, curr) => prev.length > curr.length ? prev : curr, '');
        console.timeEnd('Benchmark');
        console.log('Biggest word found:', biggestWord);
      });

  } catch (error) {
    console.error('Error:', error);
  }
};
benchmark()

export default benchmark