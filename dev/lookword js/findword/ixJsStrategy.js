import fs from 'fs';
import path from 'path';
import { promises as fsPromises } from 'fs';

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

async function countWordsInFile(filePath) {
  const fileContent = await fsPromises.readFile(filePath, 'utf-8');
  const lines = fileContent.split('\n');
  let skipLines = 14;
  
  const words = lines
    .filter((_, index) => index >= skipLines)
    .map(line => line.split(' '))
    .flat()
    .filter(word => word.length)
    .reduce((wordCounts, word) => {
      wordCounts[word] = (wordCounts[word] || 0) + 1;
      return wordCounts;
    }, {});
    
  if (lines.find(line => line.includes('*** END OF '))) {
    return words;
  } else {
    return {};
  }
}

async function countWordsInDirectory(directoryPath) {
  const files = [];
  for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
    files.push(filePath);
  }

  const wordCounts = await Promise.all(files.map(countWordsInFile));
  return wordCounts.reduce((total, currentCount) => {
    for (const [word, count] of Object.entries(currentCount)) {
      total[word] = (total[word] || 0) + count;
    }
    return total;
  }, {});
}

// Usage
async function benchmark() {
  try {
    console.time('Benchmark');
    const result = await countWordsInDirectory('F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg');  // replace with your directory path
    console.timeEnd('Benchmark');
    console.log(result);
  } catch (err) {
    console.error(err);
  }
}

benchmark();

export default benchmark;
