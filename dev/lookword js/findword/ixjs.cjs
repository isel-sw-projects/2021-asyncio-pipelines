const fs = require('fs');
const path = require('path');
const fsPromises = fs.promises;
const { AsyncIterable } = require('ix');
const { map, flatMap, filter, skip, takeWhile } = require('ix/asynciterable/operators');
const { reduce, maxBy } = require('ix/asynciterable');

const wordSeparator = ' ';;

async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await fsPromises.readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const filePath = path.resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectoryGenerator(filePath);
    } else if (dirent.isFile() && dirent.name.endsWith('.txt')) {
      yield filePath;
    }
  }
}


function findBiggestWordInFile(filePath) {
  return AsyncIterable.from(fsPromises.readFile(filePath, 'utf-8')).pipe(
    skip(14),
    takeWhile((line) => !line.includes('*** END OF ')),
    filter((line) => 
    line.length > 0),
    flatMap((line) => line.split(wordSeparator)),
    
  ).maxBy((word) => word.length);
}

async function findBiggestWordInDirectory(directoryPath) {
  let longestWord = '';
  for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
    const word = findBiggestWordInFile(filePath);
    if (word.length > longestWord.length) {
      longestWord = word;
    }
  }
  return longestWord;
}

async function benchmark() {
  const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/books/gutenberg/test';
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
