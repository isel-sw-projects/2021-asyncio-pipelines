import fs from 'fs';
import path from 'path';
import { promises as fsPromises } from 'fs';
import { Iterable } from 'ix';

async function* getFilesFromDirectoryGenerator(dir) {
    const dirents = await fsPromises.readdir(dir, { withFileTypes: true });
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
    const fileContent = fs.readFileSync(filePath, 'utf-8');
    const lines = fileContent.split('\n');
    let skipLines = 14;
    
    return Iterable.from(lines)
      .filter(() => skipLines-- > 0 ? false : true)
      .takeWhile(line => !line.includes('*** END OF '))
      .flatMap(line => line.split(' '))
      .reduce((prev, curr) => prev.length > curr.length ? prev : curr, '');
  }

async function findBiggestWordInDirectory(directoryPath) {
  let biggestWord = '';
  for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
    const biggestWordInFile = findBiggestWordInFile(filePath);
    if(biggestWordInFile.length > biggestWord.length) {
      biggestWord = biggestWordInFile;
    }
  }
  return biggestWord;
}

async function benchmark(){
  const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
  try {
    console.log('Starting benchmark baseline with all file to memory:');
    console.time('Benchmark');
    const biggestWord =  await findBiggestWordInDirectory(folderPath);
    console.log(biggestWord)
    console.timeEnd('Benchmark');
    console.log('Biggest word found:', biggestWord);
  } catch (error) {
    console.error('Error:', error);
  }
};
benchmark()
export default benchmark