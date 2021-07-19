# Telegram Bots Book

[![Build Status](https://img.shields.io/travis/com/TelegramBots/book/master?style=flat-square)](https://travis-ci.com/TelegramBots/book)
[![Demo](https://img.shields.io/badge/live-demo-blue.svg?style=flat-square)](https://telegrambots.github.io/book/)

This repository contains documentation for TelegramBots projects.
Book is a great example-based tutorial for developing Telegram chat bots 🤖.

## 🔨 Build & Test ✔

This book is a web app generated from markdown files using [mdBook] tool.
Each file mentioned in [SUMMARY](src/SUMMARY.md) will be a HTML page.

1. Install [mdBook] v0.4.10
    - You can use a [mdBook binary]
    - Or install it using Rust package manager

        ```bash
        cargo install mdbook --vers "^0.4.10"
        ```

1. Run locally on [http://localhost:3000](http://localhost:3000)

    ```bash
    mdbook serve
    ```

[mdBook]: https://github.com/rust-lang/mdBook
[mdBook binary]: https://github.com/rust-lang/mdBook/releases/tag/v0.4.1

## Contribute 👋

**Your contribution is welcome!** 🙂
See [Contribution Guidelines](CONTRIBUTING.md).
