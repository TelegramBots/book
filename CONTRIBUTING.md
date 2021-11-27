# Contributing

This book 📖 serves as a detailed guide in developing Telegram Bots.
**Your contribution is welcome!** 🙂

## 🔨 Tools Needed

Book is generated using [mdBook].

We also check all the HTTP URLs and inter-file references during CI to avoid broken links.

## 💡 How To Contribute

1. Fork this repository
1. Commit your changes
1. Make sure all tests are passing
1. Make a pull request to `develop` branch here

## 📜 Contribution Rules

There are a few simple rules in place to keep the project easily manageable and limit merge conflicts.

- Try to edit _one chapter only_ in each commit/PR. Why?
  - Scope of change is smaller so there will be less conflicts
  - Allows us to cherry-pick commits instead of merging the whole PR if necessary
- Each chapter should have its own `docs/` directory. Why?
  - To narrow scope of changes
- Files used in code snippets should be in [`src/docs/`](src/docs/) directory and NOT in a chapter directory. Why?
  - Because of this next rule:
- Do not remove/rename files in [`src/docs/`](src/docs/). Only add files to this directory. Why?
  - They are used as hard coded values in examples in the book
- Files in `docs` directories should be named following `{type}-{file_name}.{extension}` pattern.
  - Values for type: `photo`, `shot`, `sticker`, `anim`, `video`, `voice`, `audio`, `doc`, `thumb`

[mdBook]: https://github.com/rust-lang/mdBook
