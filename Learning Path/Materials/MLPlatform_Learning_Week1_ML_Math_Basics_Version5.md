# Week 1 — ML Math Basics (Explanation-first)

## Goal
Learn the “postprocessing math” that appears in almost every classifier pipeline:
- logits vs probabilities
- stable softmax (numerically safe)
- ArgMax and TopK (with deterministic tie-breaking)
- parse numeric lists from text with **commas + spaces**
- prove correctness with xUnit tests

No ONNX, no API, no Docker this week.

---

## Concepts (explanation)

### 1) Logits vs probabilities
Most classifiers output **logits**: raw scores for each class.
- can be any real numbers (negative, large, etc.)
- larger logit ⇒ more likely class
- logits are not probabilities (not 0..1, don’t sum to 1)

To turn logits into probabilities, we use **softmax**.

---

### 2) Softmax (what it does)
Softmax converts logits into probabilities:

For each index `i`:
- `p[i] = exp(logit[i]) / sum_j exp(logit[j])`

Properties:
- `0 < p[i] < 1`
- `sum(p) = 1`
- preserves ordering:
  - `ArgMax(logits) == ArgMax(p)`

---

### 3) Why naive softmax breaks (overflow/NaN)
`exp(x)` grows extremely fast.
- with large logits like `[1000,1001,1002]`, `exp(1000)` overflows → `Infinity`
- then you get `Infinity / Infinity` → `NaN`

So the *computation* fails even though the math is correct.

---

### 4) Stable softmax (the standard fix)
Compute softmax with subtract-max:

1. `m = max(logits)`
2. `p[i] = exp(logit[i] - m) / sum_j exp(logit[j] - m)`

Why this works:
- subtracting the same constant from all logits does not change final probabilities
- largest adjusted logit becomes 0 ⇒ `exp(0)=1` (safe)
- others are ≤ 0 ⇒ exponentials are in (0,1] (safe)

---

### 5) ArgMax and TopK
**ArgMax**
- returns the index of the maximum value
- tie rule: return the first max index (lowest index)

**TopK**
- returns the top K items `(index, score)`
- sorting rule:
  1) score descending  
  2) index ascending (tie-break)

This makes output deterministic and tests stable.

---

## Exercises (do in order)

### Exercise 1 — Parse numbers (commas + spaces)
Implement a helper that parses:
- `"1,2,3"`
- `"1, 2, 3"`
- `"1 2 3"`
- `"  1,  2   3  "`

into `float[] { 1, 2, 3 }`.

Rules:
- separators: commas and/or whitespace
- ignore extra whitespace
- reject empty input or invalid tokens with a clear exception

**Tests (minimum)**
- 6–10 tests covering valid and invalid cases.

**Done when**
- parsing is reliable and error messages are clear.

---

### Exercise 2 — Naive softmax (intentionally unsafe)
Implement naive softmax and write a test that demonstrates it can fail on large logits:
- input: `[1000,1001,1002]`
- expectation: “all outputs are finite” (this should fail or reveal NaN/Inf risk)

**Done when**
- you can show naive softmax is unsafe (via a failing test or an explicit NaN/Inf detection).

---

### Exercise 3 — Stable softmax (fix)
Implement stable softmax (subtract-max).

**Tests (minimum)**
- outputs are finite for `[1000,1001,1002]`
- sum of probabilities ≈ 1 (tolerance like `1e-5`)

**Done when**
- stable softmax passes all tests.

---

### Exercise 4 — ArgMax
Implement `ArgMax(values)`.

**Tests**
- normal case
- negative values
- ties return first index

**Done when**
- ArgMax behavior is correct and tested.

---

### Exercise 5 — TopK (support both Clamp and Strict)
Implement two explicit methods:

**TopKClamp(values, k)**
- if `k > length`: clamp to `length` (return all)
- if `k <= 0`: throw `ArgumentOutOfRangeException`

**TopKStrict(values, k)**
- if `k > length`: throw `ArgumentOutOfRangeException`
- if `k <= 0`: throw `ArgumentOutOfRangeException`

Both share sorting:
- score desc, index asc

**Tests (minimum)**
- ties deterministic
- clamp returns all when k > length
- strict throws when k > length
- both throw when k <= 0

**Done when**
- behavior is unambiguous, deterministic, and proven by tests.

---

## Suggested pacing (slow and steady)
- Day 1–2: parsing + tests
- Day 3: naive softmax + failing/diagnostic test
- Day 4: stable softmax + tests
- Day 5: ArgMax + TopK + tests
- Day 6–7: cleanup, rerun tests, write a short note explaining softmax stability

---

## Week 1 “Done when” checklist
- [ ] comma+space parsing implemented and tested
- [ ] naive softmax implemented and shown to be unsafe
- [ ] stable softmax implemented and tested (finite + sum≈1)
- [ ] ArgMax implemented and tested
- [ ] TopKClamp and TopKStrict implemented and tested
- [ ] `dotnet test` is green

---

## What to send me when you finish Week 1
- your public method signatures (just signatures)
- the large-logits test result (did naive softmax fail on your machine?)
- any part that felt confusing (parsing, float tolerances, tie-breaking)