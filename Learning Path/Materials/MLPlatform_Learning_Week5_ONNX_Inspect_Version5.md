# Week 5 — ONNX Runtime: Inspect a Model (Explanation-first)

## Goal
Make your first reliable contact with ONNX models by learning to inspect:
- input names
- input element types
- input dimensions
- output names/types/dimensions

No inference yet. Inspection first.

---

## Concepts (explanation)

### 1) Why inspection comes before inference
Most failures happen because of mismatches:
- wrong input name (common)
- wrong dtype (float vs int64)
- wrong shape (rank mismatch or fixed dim mismatch)

If you inspect first, you avoid guessing.

---

### 2) What you need from the model
When you inspect, you’re trying to answer:
- What is the exact input name?
- Is it float32 or int64?
- What rank and dims does it expect?
- What outputs exist?

You will use these answers later to build the correct tensors.

---

## Exercises (in order)

### Exercise 1 — Create a tiny ONNX inspect program
Make a small console app (separate from Week 4 CLI if you want) that:
- takes `--model <path>`
- loads it with ONNX Runtime
- prints inputs and outputs as:
  - `Name | Type | Dims`

Dims formatting:
- show unknown dims clearly (e.g., `?`)

**Done when**
- you can run it on any ONNX file you have and see readable output

---

### Exercise 2 — Make it a CLI subcommand (optional but recommended)
Add a command (either in your Week 4 CLI or as a standalone):
- `mlplatform onnx inspect --model path.onnx`

**Done when**
- `--help` is clear and errors are friendly

---

### Exercise 3 — Error handling
Handle:
- missing argument
- file not found
- invalid ONNX file

**Done when**
- the error messages include the path and what went wrong

---

## Extra credit (optional)
- Print a summary:
  - number of inputs
  - number of outputs
  - list of input names only (quick view)

---

## What to send me when you finish
- output of inspecting one model you have
- one question you had about dims/types you saw