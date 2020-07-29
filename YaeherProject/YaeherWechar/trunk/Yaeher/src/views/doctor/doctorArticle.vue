<template>
    <div class="doctor-article padding-top">
        <mt-header fixed title="文章">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
            <ul v-infinite-scroll="getArticleList"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="article-case">
				<li v-for="(item, index) in articleList" :key="index" class="">
			        <router-link :to="{path: '/article-detail', query: {id: item.id, consultID: item.consultID}}" class="articleList doctor-a">
						<img v-if="item.imageFie !== null" :src="item.imageFie"  alt="" />
                    	<img v-if="item.imageFie === null" src="../../assets/image/article3.jpg" alt="" />
			        	<div class="articleCase">
							<span v-if="item.checkState != 'checked'" class="hint red-hint">{{item.checkStatus}}</span>
							<span v-if="item.checkState === 'checked'" class="hint green-hint">{{item.checkStatus}}</span>
			        		<p class="blod">{{item.paperTiltle}}</p>
			        		<p class="line-text">{{item.paperContent}}</p>
							<p class="line-text">{{item.createdOn}}</p>
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
			skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
			articleList: [],
			doctorPaperState: [],
			loading: true
        }
	},
	methods: {
		getArticleList () { // 获取文章列表
			this.loading = true
			const skipCount = this.skipCount
			const maxResultCount = this.maxResultCount
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.instance.doctorPaperPageD({
				skipCount,
				maxResultCount
            })
                .then((response) => {
					setTimeout(() => {
						this.loading = false
						let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
						let articleList1 = response.data.result.item.items
						this.articleList = this.articleList.concat(articleList1)
						for (let i = 0; i < this.articleList.length; i++) {
							this.articleList[i].paperContent = this.articleList[i].paperContent.replace(/<[^>]+>|&/g,"").replace(/nbsp;/g,"")
							this.articleList[i].createdOn = moment(this.articleList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
						}
						this.skipCount ++
						this.totalPage = response.data.result.item.totalPage
					}, 100);	
                })
                .catch((error) => {
					this.loading = true
                }) 
		}
	},
	mounted () {
		this.getArticleList()
	},
	created () {
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctor-article {
	.listAbout {position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;}
	.article-case {
		margin-top: 10px;
		li {
			padding: 15px 0; 
			display: flex;
			.doctor-a {
				display: block;
				width: 100%;
				display: flex;
				img{
					display: block;
					width: 60px;
					height: 60px;
					border-radius: 5px;
				}
				.articleCase {
					position: relative;
					flex: 1;
					font-size: $font-l;
					.hint {
						position: absolute;
						right: 0;
						top: 0;
					}
					.blod {
						font-size: $font-xl; 
						color: $color-afont;
					}
					.line-text {
						width: 230px; 
						white-space: nowrap; 
						overflow: hidden; 
						text-overflow: ellipsis;
					}
				}
			}
		}
	}
}

</style>