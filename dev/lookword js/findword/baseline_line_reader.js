import fs from 'fs';
import readline from 'readline';
import path from 'path';
import { promises as fspromises } from 'fs';

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
  const fileStream = fs.createReadStream(filePath);
  const rl = readline.createInterface({
    input: fileStream,
    crlfDelay: Infinity,
  });

  let biggestWord = '';
  let lineCounter = 0;
  for await (const line of rl) {
    lineCounter++;

    if (lineCounter <= 14) {
      continue;
    }

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
   const folderPath = process.env.TESE_BOOKS_FOLDER_PATH || 'default/path/if/not/set';
  try {
    console.log('Starting benchmark baseline with line reader:');
    console.time('Benchmark');
    const biggestWord = await findBiggestWordInDirectory(folderPath);
    console.timeEnd('Benchmark');
    console.log('Biggest word found:', biggestWord);
  } catch (error) {
    console.error('Error:', error);
  }
})();
