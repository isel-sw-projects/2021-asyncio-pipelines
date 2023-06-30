import baseline_pipeline from './countword/baseline_pipeline.js';
//import ixJsStrategy_benchmark from './countword/ixjs.js';
import rxStrategy_benchmark from './countword/rxjs.js';
import baseline_benchmark from './countword/baseline.js';

// Array to store the benchmark functions and their names
const benchmarks = [
  { name: 'baseline_pipeline_js', benchmark: baseline_pipeline },
 // { name: 'ixJsStrategy', benchmark: ixJsStrategy_benchmark },
  { name: 'rxStrategy', benchmark: rxStrategy_benchmark },
  { name: 'baseline', benchmark: baseline_benchmark },
];


async function runBenchmark(benchmarkObj, runs) {
  const durations = [];
  const wordCounts = [];


  for (let i = 0; i < runs; i++) {
    const start = process.hrtime.bigint();
    const wordCount = await benchmarkObj.benchmark().catch(e => console.error(`Benchmark error: ${e}`));
    const end = process.hrtime.bigint();


    durations.push(Number(end - start) / 1e6);
    wordCounts.push(wordCount);
  }

  // Discard first run and calculate average duration of last 3 runs
  durations.shift();
  const avgDuration = durations.reduce((sum, duration) => sum + duration, 0) / (runs - 1);

  // Calculate the most common word between 5 and 10 characters long
  const allWords = {};
  for (let wordCount of wordCounts) {
    Object.assign(allWords, wordCount);
  }

  const mostCommonWord = Object.entries(allWords)
    .filter(([word]) => word.length >= 5 && word.length <= 10)
    .sort((a, b) => b[1] - a[1])[0] || ["", 0]; // Added fallback in case no common word was found

  return { name: benchmarkObj.name, avgDuration, mostCommonWord };
}

async function runAllBenchmarks() {
  for (const benchmarkObj of benchmarks) {
    const { name, avgDuration, mostCommonWord } = await runBenchmark(benchmarkObj, 4);
    console.log(`Average duration for ${name}: ${avgDuration.toFixed(2)} ms`);
    console.log(`Most common word for ${name}: ${mostCommonWord[0]} (${mostCommonWord[1]} occurrences)`);
  }
}

runAllBenchmarks();
