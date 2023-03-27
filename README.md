# Telegram Bots Book

[![book](https://img.shields.io/badge/TelegramBots-Book-blue.svg?style=flat)](https://telegrambots.github.io/book/)
[![master](https://github.com/TelegramBots/book/actions/workflows/ci.yml/badge.svg)](https://github.com/TelegramBots/book/actions/workflows/ci.yml)

This repository contains documentation for [TelegramBots](https://github.com/TelegramBots) projects.
Book is a great tutorial on writing Telegram bots in .NET ecosystem 🤖.

## 🔨 Build & Test ✔

This book is a web app generated from markdown files with [mdBook].
Each markdown file mentioned in [SUMMARY](src/SUMMARY.md) will be rendered as an HTML page.

1. Install [mdBook]:
    - You can download a [mdBook binary]
    - Or install it using Rust package manager

      ```bash
      cargo install mdbook --vers "^0.4.28"
      ```

1. Run locally at [localhost:3000](http://localhost:3000):

    ```bash
    mdbook serve --open
    ```

## Contribute 👋

**Your contribution is welcome!** 🙂
See [Contribution Guidelines].

<!-- -->

[mdBook]: https://github.com/rust-lang/mdBook
[mdBook binary]: https://github.com/rust-lang/mdBook/releases
[Contribution Guidelines]: CONTRIBUTING.md
