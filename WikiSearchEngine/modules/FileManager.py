from pathlib import Path
import nltk


def readFile(path: str) -> str:
    f = open(path, "r")
    content = f.read()
    f.close()
    return content


def writeFile(content: str, path: str):
    f = open(path, "w+")
    f.write(content)
    f.close()


def writeAppend(content: str, path: str):
    f = open(path, "a")
    f.write(content)
    f.close()


def tokenize(text: str) -> list[str]:
    return nltk.sent_tokenize(text)
