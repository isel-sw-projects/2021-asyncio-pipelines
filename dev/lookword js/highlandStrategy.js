import fs from 'fs';
import path from 'path';
import hl from 'highland';

function splitBy(separator) {
  return hl.flatMap((str) => hl(str.split(separator)));
}

const readFile = hl.wrapCallback(fs.readFile);

async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await fs.promises.readdir(dir, { withFileTypes: true });
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
  return readFile(filePath, 'utf-8')
    .splitBy('\n')
    .drop(14)
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
  
    return hl(files)
      .flatMap(findBiggestWordInFile)
      .reduce((biggest, curr) => (curr.length > biggest.length ? curr : biggest), '')
      .last();
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
