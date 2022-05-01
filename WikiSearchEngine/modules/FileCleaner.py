from bs4 import BeautifulSoup  # czyszczenie html

import nltk  # tokenizing
from nltk.stem import LancasterStemmer  # stemming
from nltk.corpus import stopwords  # stopwords
import typing  # typy
import re  # modyfikowanie stringow

StopWordsDict = set(stopwords.words("english"))
Stemmer = LancasterStemmer()
WordsForNormalization = 300


def cleanHtml(html: str) -> str:
    html = BeautifulSoup(html, "lxml").text
    html = re.sub(r"http[^\ ]*\ ", r" ", html)
    html = re.sub(
        r"\—|\%|\$|\'|\||\.|\*|\[|\]|\:|\;|\,|\{|\}|\(|\)|\=|\+|\-|\_|\#|\!|\`|\"|\?|\/|\>|\<|\&|\\|\u2013|\n",
        r" ",
        html,
    )
    return html


def makeLowerCase(text: str) -> str:
    return text.lower()


def stem(text: list[str]) -> list[str]:
    stemmed, size = [], 0
    for word in text:
        if word not in StopWordsDict:
            stemmed.append(Stemmer.stem(word))
            size += 1
        if size >= WordsForNormalization:
            break
    return stemmed
