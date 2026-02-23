# Week 2 — Tensors & Shapes (Explanation-first)

## Goal
Understand tensors as **flat arrays + shape**, and become confident with:
- NCHW vs NHWC layouts
- indexing/flattening
- writing tests that prove your indexing is correct

You are not using ONNX yet.

---

## Concepts (explanation)

### 1) What a tensor really is
A tensor is just:
- `data`: a 1D array (e.g., `float[]`)
- `shape`: dimensions (e.g., `[1,3,224,224]`)

The shape tells you how to interpret positions in the 1D array.

---

### 2) Common layouts you will see later
**NCHW** (common in ONNX vision models)
- `[N, C, H, W]` = batch, channels, height, width

**NHWC** (common in other ecosystems)
- `[N, H, W, C]`

Both can represent the same conceptual image, but the memory order differs.

---

### 3) Flattening / indexing (the key skill)
You will frequently need to compute where `(n,c,h,w)` lands in the flat array.

For NCHW:
- `idx = (((n * C + c) * H + h) * W + w)`

For NHWC:
- `idx = (((n * H + h) * W + w) * C + c)`

If you get this wrong, your model gets garbage input even if shapes “look right”.

---

## Worked example (tiny)
Shape: `[N=1, C=3, H=2, W=2]` (NCHW)

Coordinates for channel 0:
- (n=0,c=0,h=0,w=0) idx = 0
- (0,0,0,1) idx = 1
- (0,0,1,0) idx = 2
- (0,0,1,1) idx = 3

Channel 1 starts at idx 4, channel 2 starts at idx 8.

That “channel blocks are contiguous” property is characteristic of NCHW.

---

## Exercises (in order)

### Exercise 1 — `TensorShape` type
Implement a small type that:
- stores dims (int[])
- provides:
  - `Rank` (dims.Length)
  - `ElementCount` (product of dims)
- rejects invalid dims (<= 0)

**Done when**
- tests cover valid shapes and invalid shapes

---

### Exercise 2 — Indexing functions
Implement:
- `IndexNchw(n,c,h,w, C,H,W) -> int`
- `IndexNhwc(n,h,w,c, H,W,C) -> int`

**Done when**
- tests cover a few known coordinates and expected indices

---

### Exercise 3 — Prove “no duplicates, no out of range”
For a small shape (e.g., `[1,3,2,2]`):
- iterate all coordinates
- compute indices
- verify:
  - every index is between `0` and `ElementCount-1`
  - the set of indices has exactly `ElementCount` unique values

**Done when**
- a single test proves your indexing covers every element exactly once

---

### Exercise 4 — Fill with a pattern and verify
Create a tensor `float[] data` for NCHW `[1,3,2,2]` and fill:
- channel 0 all `1`
- channel 1 all `2`
- channel 2 all `3`

Verify:
- first 4 values are 1
- next 4 values are 2
- last 4 values are 3

**Done when**
- tests pass and you can explain why this is true for NCHW

---

## Extra credit (optional)
- Add a `Layout` enum (NCHW/NHWC)
- Write a single helper that computes indices based on layout choice

---

## What to send me when you finish
- Your indexing formulas (as code or plain text)
- The test you wrote for “no duplicates”
- One sentence explaining NCHW vs NHWC in your own words