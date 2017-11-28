#coding=utf-8
'''
字符串反转
'''
import re
def wordReverse(path,path2):
    with open(path) as file:
        with open(path2,'w') as file2:
            passage=file.read();
            re_passage=''
            word=''
            for char in passage:
                if re.match(r"[a-zA-Z]",char):
                    word=char+word
                else:
                    re_passage=re_passage+word+char
                    word=''
            print(re_passage)
            file2.write(re_passage)
        file.close()
        file2.close()
wordReverse('A.txt','B.txt')
