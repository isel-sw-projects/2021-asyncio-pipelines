import baseline_benchmark from './findword/baseline_js.js';
import baseline_stream_benchmark from './findword/baseline_array_pipeline.js';
import rxStrategy_benchmark from './findword/rxStrategy.js';

// Array to store the benchmark functions and their names
const benchmarks = [
  { name: 'baseline_js', benchmark: baseline_benchmark },
  { name: 'baseline_stream', benchmark: baseline_stream_benchmark },
  { name: 'rxStrategy', benchmark: rxStrategy_benchmark },
];

// Function to run benchmark functions multiple times and calculate the average duration
async function runBenchmark(benchmarkObj, runs) {
  const durations = [];

  for (let i = 0; i < runs; i++) {
    const start = process.hrtime.bigint();
    await benchmarkObj.benchmark();
    const end = process.hrtime.bigint();

    // We discard the first run duration
    if (i !== 0) {
      durations.push(Number(end - start) / 1e6);
    }
  }

  const avgDuration = durations.reduce((sum, duration) => sum + duration, 0) / (runs - 1);
  return { name: benchmarkObj.name, avgDuration };
}

async function runAllBenchmarks() {
  for (const benchmarkObj of benchmarks) {
    const { name, avgDuration } = await runBenchmark(benchmarkObj, 4);  // incrementing run count by 1 as we discard the first run
    console.log(`Average duration for ${name}: ${avgDuration.toFixed(2)} ms`);
  }
}

runAllBenchmarks();
