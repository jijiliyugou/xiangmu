<template>
    <div class="article-list padding-top">
		<mt-header fixed title="文章列表">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
			<div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入关键字查询" />
                    <div @click="getArticleList(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getArticleList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10"  class="article-case">
				<li v-for="(item, index) in articleList" :key="index" class="">
			        <router-link :to="{path: '/article-detail', query: {id: item.id}}" class="articleList">
						<img v-if="item.imageFie !== null" :src="item.imageFie"  alt="" />
                    	<img v-if="item.imageFie === null" src="../../assets/image/article3.jpg" alt="" />
			        	<div class="articleCase">
			        		<p class="blod">{{item.paperTiltle}}</p>
			        		<p class="line-text">{{item.paperContent}}</p>
			        	</div>
			        </router-link>
			    </li>
			</ul>
        </div>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
			id: 0,
			keyWord: '',
			skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
			articleList: [],
			loading: true,
			checkState: ''
        }
	},
	methods: {
		getArticleList (flag) {
			this.loading = true
			const doctorId = this.id
			if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
			const keyWord = this.keyWord
			const skipCount = this.skipCount
			const maxResultCount = this.maxResultCount
			const checkState = this.checkState
			if(skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.doctorPaperPage({
				doctorId,
				keyWord,
				skipCount,
				maxResultCount,
				checkState
            })
                .then((response) => {
					setTimeout(() => {
						this.loading = false
						let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
						if (flag) this.articleList = []
						let articleList1 = response.data.result.item.items
						this.articleList = this.articleList.concat(articleList1)
						for (let i = 0; i < this.articleList.length; i++) {
							this.articleList[i].paperContent = this.articleList[i].paperContent.replace(/<[^>]+>|&/g,"").replace(/nbsp;/g,"")
						}
						this.skipCount ++
						this.totalPage = response.data.result.item.totalPage
					}, 100);	
                })
                .catch((error) => {
					this.loading = true
                }) 
		},
		searchKeypress() {
            if (event.keyCode == 13) {
                this.getArticleList(true)
            } 
        },
	},
	mounted () {
		this.getArticleList()
	},
	created () {
		this.id = parseInt(this.$route.query.id)

		let checkState = this.$route.query.checkState
		if (checkState) {
			this.checkState = checkState
		}
		
		console.log(this.checkState)
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.article-list {
	.listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
	.article-case {
		margin-top: 10px;
		li {
			padding: 15px 0; 
			display: flex;
			.doctor-a {
				display: block;
				width: 100%;
				.doctorInfo {
					img{
						display: block;
						width: 60px;
						height: 60px;
						border-radius: 5px;
					}
					.flexCase {
						padding: 0 10px; 
						p {
							font-size: $font-m;
							color: $color-bfont;
							padding-bottom: 2px;
						}
						.doctor-name {
							font-size: $font-xl;
							color: $color-afont;
						}
						.star-font {
							color: $color-star;
						}
					}
				}  
			}
		}
	}
	.articleList {overflow: hidden; display: flex; position: relative;}
	.articleList img {float: left; width: 60px; height: 60px; border-radius: 8px;}
	.articleList .articleCase {padding: 0 10px;}
	.articleCase p {font-size: $font-l; color: $color-bfont;}
	.articleCase .blod {font-size: $font-xl; color: $color-afont;}
	.articleCase .line-text {width: 250px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;}
}

</style>