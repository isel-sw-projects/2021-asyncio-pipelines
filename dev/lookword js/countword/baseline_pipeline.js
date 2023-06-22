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

async function countWordsInFile(filePath, minLength = 5, maxLength = 10) {
    const fileContent = await fsPromises.readFile(filePath, 'utf-8');
    const lines = fileContent.split('\n');

    let skipLines = 14;
    let continueCounting = true;

    return lines
        .filter(line => {
            if (line.includes('*** END OF ')) {
                continueCounting = false;
            }
            skipLines--;
            return skipLines <= 0 && continueCounting;
        })
        .flatMap(line => line.split(' '))
        .filter(word => word !== '' && word.length > minLength && word.length < maxLength)
        .reduce((wordCount, word) => {
            wordCount[word] = (wordCount[word] || 0) + 1;
            return wordCount;
        }, {});
}

async function countWordsInDirectory(directoryPath) {
    const wordCounts = {};
    for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
        const fileWordCounts = await countWordsInFile(filePath);
        for (const [word, count] of Object.entries(fileWordCounts)) {
            wordCounts[word] = (wordCounts[word] || 0) + count;
        }
    }
    return wordCounts;
}

async function benchmark() {
    const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
    try {
        console.log('Starting pipeline benchmark:');
        console.time('Pipeline Benchmark');
        const wordCounts = await countWordsInDirectory(folderPath);
        console.timeEnd('Pipeline Benchmark');
    } catch (error) {
        console.error('Error:', error);
    }
}

export default benchmark;
