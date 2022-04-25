# Telegram Bots Book

[![book](https://img.shields.io/badge/TelegramBots-Book-blue.svg?style=flat)](https://telegrambots.github.io/book/)
[![master](https://github.com/TelegramBots/book/actions/workflows/ci.yml/badge.svg)](https://github.com/TelegramBots/book/actions/workflows/ci.yml)

This repository contains documentation for TelegramBots projects.
Book is a great example-based tutorial for developing Telegram chat bots 🤖.

## 🔨 Build & Test ✔

This book is a web app generated from markdown files using [mdBook] tool.
Each file mentioned in [SUMMARY](src/SUMMARY.md) will be a HTML page.

1. Install [mdBook] v0.4.18
    - You can use a [mdBook binary]
    - Or install it using Rust package manager

        ```bash
        cargo install mdbook --vers "^0.4.18"
        ```

1. Run locally on [http://localhost:3000](http://localhost:3000)

    ```bash
    mdbook serve
    ```

## Contribute 👋

**Your contribution is welcome!** 🙂
See [Contribution Guidelines].

<!-- -->

[mdBook]: https://github.com/rust-lang/mdBook
[mdBook binary]: https://github.com/rust-lang/mdBook/releases/tag/v0.4.18
[Contribution Guidelines]: CONTRIBUTING.md
