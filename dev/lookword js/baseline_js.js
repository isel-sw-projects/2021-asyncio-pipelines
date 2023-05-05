import fs from 'fs';
import path from 'path';
import { promisify } from 'util';
import { promises as fspromises } from 'fs';

const readFile = promisify(fs.readFile);

async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await fspromises.readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const filePath = path.resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectoryGenerator(filePath);
    } else if (dirent.name.endsWith('.txt')) {
      yield filePath;
    }
  }
}

async function findBiggestWordInFile(filePath) {
  const fileContent = await readFile(filePath, 'utf-8');
  const lines = fileContent.split('\n');

  let biggestWord = '';
  for (let i = 14; i < lines.length; i++) {
    const line = lines[i];
    if (line.includes('*** END OF ')) {
      break;
    }

    const words = line.split(' ');
    for (const word of words) {
      if (word.length > biggestWord.length) {
        biggestWord = word;
      }
    }
  }

  return biggestWord;
}

async function findBiggestWordInDirectory(directoryPath) {
  const files = [];
  for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
    files.push(filePath);
  }

  const findBiggestWordPromises = files.map(async (filePath) => {
    const biggestWordInFile = await findBiggestWordInFile(filePath);
    return biggestWordInFile;
  });

  const biggestWords = await Promise.all(findBiggestWordPromises);

  return biggestWords.reduce((prev, curr) => {
    return prev.length > curr.length ? prev : curr;
  }, '');
}


(async () => {
  const folderPath = 'C:/Users/e351582/OneDrive - EDP/Desktop/PESSOAL/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
  try {
    console.log('Starting benchmark baseline with all file to memory:');
    console.time('Benchmark');
    const biggestWord = await findBiggestWordInDirectory(folderPath);
    console.timeEnd('Benchmark');
    console.log('Biggest word found:', biggestWord);
  } catch (error) {
    console.error('Error:', error);
  }
})();
