# Week 6 (Optional) — ONNX Runtime: Run One Inference (Explanation-first)

Do this only if you have:
- at least one ONNX model file available, and
- you can inspect it successfully (Week 5 done)

This is a **smoke test** week: learn the mechanics and the common errors.

---

## Goal
Run a single inference by:
- building an input tensor with correct name/dtype/shape
- executing ONNX Runtime
- printing outputs
- (optionally) applying softmax + top-k if output looks like logits

---

## Concepts (explanation)

### 1) Input tensor must match the contract exactly
You must match:
- input name (exact string)
- element type (float32 vs int64)
- rank and dimensions

If you mismatch, ORT will throw.

---

### 2) “Fill tensor with a constant” is enough for a smoke test
For learning mechanics, you don’t need real data:
- create tensor filled with 0.5 or 1.0
- the output may be meaningless, but the goal is: it runs and you can inspect shapes.

---

### 3) Output interpretation
Outputs might be:
- logits vector (common for classification)
- multiple outputs
- something else (depends on model)

For this week:
- just print output names and shapes
- print first N values of the first output
- if it’s a 1D logits-like vector, you can apply stable softmax + top-k

---

## Exercises (in order)

### Exercise 1 — Minimal “run” program
Write a console app that:
- takes:
  - `--model path.onnx`
  - `--inputName <name>`
  - `--shape "1,3"` (or more dims)
  - `--fill 1.0`
- creates a float tensor with that shape, filled with `fill`
- runs inference
- prints:
  - output name(s)
  - output dim(s)
  - first 10 values (or fewer if smaller)

**Done when**
- you can run the program and get outputs without exceptions (for at least one model)

---

### Exercise 2 — Diagnose the top 3 failure types (intentionally)
Run your program with:
1. wrong input name → observe error
2. wrong rank/shape → observe error
3. wrong dtype (if your model uses int64) → observe error

Write down what each error looks like and what it means.

**Done when**
- you can look at an ORT error and immediately suspect name vs dtype vs shape

---

### Exercise 3 — Optional: softmax + top-k on output
If output is a 1D vector (or can be flattened to one):
- apply Week 1 stable softmax
- print top-5

**Done when**
- you can produce “top predictions” from raw output values (even if labels are unknown)

---

## What to send me when you finish
- the command you used to run inference
- the printed input/output shapes
- the 3 errors you observed (name, shape, dtype) and what you learned