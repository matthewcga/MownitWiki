
from collections import defaultdict  # slownik wyrazow
import math  # log
import typing  # typy
import decimal  # decimal

import FileManager as fm  # read/write plikow


defDictSize = decimal.Decimal(len(defaultdict(list)))


def getTF(words: list[str]) -> dict[str, decimal.Decimal]:
    """
    oblicza 'term frequency'
    dla 1 dokumentu
    """
    tf = dict.fromkeys(defaultdict(list), decimal.Decimal(0))  # slownik wystapien slow
    for word in words:
        tf[word] += decimal.Decimal(1)

    for word, freq in tf.items():
        tf[word] = freq / defaultdict

    return tf


def getITF(documents: list[str]) -> dict[str, decimal.Decimal]:
    """
    oblicza 'inverse term frequency'
    dla wszystkich dokuemntow
    by oszczedzic pamiec kazy jest pojedynczo wczytywany
    """
    idfDict, n = dict.fromkeys(documents[0].keys(), 0), decimal.Decimal(len(documents))

    for document in documents:
        tf = getTf(fm.tokenize(fm.readFile(document)))
        for word, freq in tf.items():
            if freq > 0:
                idfDict[word] += 1

    for word, freq in idfDict.items():
        idfDict[word] = decimal.Decimal(n / freq).log10

    return idfDict


def computeTFIDF(tf, idf) -> dict[str, decimal.Decimal]:
    """
    tfidf dla to tf * idf
    dla 1 dokumentu
    """

    for word, freq in tf.items():
        tf = freq * idf[word]
    
    return tf

def exportTFIDF(path: str, tfidf: dict[str, decimal.Decimal], id: int):
    f = open(path, "w+")
    for freq in tfidf.values():
        f.write("%d," % (freq.to_eng_string()))
    f.close()
