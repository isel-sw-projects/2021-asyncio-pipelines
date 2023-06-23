const from = require('ix/iterable').from;
const { filter, map, reduce  } = require('ix/iterable/operators');

const source = function* () {
  yield 1;
  yield 2;
  yield 3;
  yield 4;
};

const results = from(source()).pipe(
  filter(x => x % 2 === 0),
  map(x => x * x)
);

for (let item of results) {
  console.log(`Next: ${item}`);
}

// Next 4
// Next 16